resource "aws_route53_zone" "oars_zone" {
  name = var.domain_name
}

# resource "aws_route53_record" "oars_cert_dns" {
#   allow_overwrite = true
#   name =  tolist(aws_acm_certificate.oars_cert.domain_validation_options)[0].resource_record_name
#   records = [tolist(aws_acm_certificate.oars_cert.domain_validation_options)[0].resource_record_value]
#   type = tolist(aws_acm_certificate.oars_cert.domain_validation_options)[0].resource_record_type
#   zone_id = aws_route53_zone.oars_zone.zone_id
#   ttl = 60
# }

resource "aws_route53_record" "oars_alb_dns" {
  name = var.domain_name
  type = "A"
  zone_id = aws_route53_zone.oars_zone.zone_id
  alias {
    name                   = aws_lb.oars_alb.dns_name
    zone_id                = aws_lb.oars_alb.zone_id
    evaluate_target_health = true
  }
}