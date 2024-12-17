terraform {
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 4.16"
    }
  }

  required_version = ">= 1.2.0"
}

provider "aws" {
  region = "us-west-2" # Altere para sua região, se necessário
}

resource "aws_instance" "app_server" {
  ami           = "ami-830c94e3" # ID da AMI para a região us-west-2
  instance_type = "t2.micro"

  tags = {
    Name = "ExampleAppServerInstance"
  }
}