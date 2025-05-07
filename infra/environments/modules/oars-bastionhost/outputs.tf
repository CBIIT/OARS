output "public_ip" {
  value = aws_instance.bastion_ec2.public_ip  
}