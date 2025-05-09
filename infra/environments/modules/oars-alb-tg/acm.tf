resource "aws_acm_certificate" "oars_cert" {
  domain_name       = var.domain_name_cert
  validation_method = "DNS"
  lifecycle {
    create_before_destroy = true
  }
}
resource "aws_acm_certificate_validation" "oars_cert_validate" {
  certificate_arn = aws_acm_certificate.oars_cert.arn
  validation_record_fqdns = [aws_route53_record.oars_cert_dns.fqdn]
}