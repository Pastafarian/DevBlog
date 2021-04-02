## About The Project
Stephen Adam's Developer Home Page


# Motivation
A simple blogging / home page application implemented in Angualar, ASP.NET Core, NGINX and Docker. 

Designed to be a good starting point and reference for anyone looking to create a .NET / Angular / Postgres / Docker application hosted on a VM in the cloud.  

### Built With
* [ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/?view=aspnetcore-5.0)
* [Angular 11](https://angular.io/)
* [Postgres](https://www.postgresql.org/)
* [Docker](https://www.docker.com/)


# Features
* Designed to be run easily on Google Cloud Platform for cheap cloud hosting
* CMS featuring image uploads and Quill rich editor for article creation.
* Full responsive mobile layout.
* Automatic HTTPS certification and renewal with lets encrypt 
* Authentication provided by Auth0 free tier

# Project Structure

## client
This is an angular 11 project. Build this in production mode and upload to a bucket. 

## server
This is an .NET Core Web Api which provides the back-end for this project.

## server files
These are the files you'll need to place on the VM. The docker-compose to pull down the images and the nginx configuration to serve the content.


# Roadmap
* Create full guide on how to deploy the site to a new server. 
* Create a white label version
* Improve unit test coverage 
* Polish administration side of the application

# Resources
https://medium.com/@pentacent/nginx-and-lets-encrypt-with-docker-in-less-than-5-minutes-b4b8a60d3a71


## Acknowledgements
* [EFCore](https://docs.microsoft.com/en-us/ef/core/)
* [AutoMapper](https://automapper.org/)
* [MediatR](https://github.com/jbogard/MediatR)
* [Serilog](https://serilog.net/)
* [SimpleInjector](https://simpleinjector.org/)
* [ImageSharp](https://github.com/SixLabors/ImageSharp)
* [Swashbuckle](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)
* [Seq](https://datalust.co/seq)
* [FluentValidation](https://fluentvalidation.net/)
* [Moq](https://github.com/moq/moq4)
* [XUnit](https://xunit.net/)
* [NGXS](https://www.ngxs.io/)
* [Quill](https://quilljs.com/)
* [NGX-Quill](https://github.com/KillerCodeMonkey/ngx-quill)
* [Auth0](https://auth0.com/)



# Deployment instructions for Google Cloud

This a simple and currently incomplete set of instructions to get up and running quickly.


## Create a new google cloud platform account - this will give you $300 for 3 months and let's you get going for free

## Create a new VM
Compute Engine -> VM Instances -> Create

* Select Debian as the base image
* Ensure you enable http and https access and select one of the smaller instances.

* Setup static IP
Go to VPC network. In External addresses click 'Reserve static address'. Ensure its attached to the instance you created.

* Setup your A record for you domain to point at the static IP

## Setup WinSCP to copy files to VM - 

* Download PuTTYgen, open it, click 'generate', update the 'key comment' to your google username. 
The username will appear when you connect to the VM via the SSH option in the web interface. I.e. username@vm-name.
* Copy the contents of the box 'Public key for pasting into OpenSSH authorized_keys file' and save to a file. 
* Click the 'Save key' button and save your private key somwhere safe. 
* In the Compute Engine section of GCP select 'Metadata' in Settings
* Copy the contents of the 'Public key for pasting into OpenSSH authorized_keys file' you saved earlier into the SSH Keys section of Metadata.
* Open WinSCP and select 'New Session'. Enter the VM's external IP into the 'Host name' box, enter your user name from above into 'User name'.
* Click advanced, select Authentication, click the '...' next to the private key box and select the private key you generated earlier 'whatever.pkk'. Click Ok. Then save. Then login.      

## Install docker on the VM


* SSH into the VM.
* Install Docker on remote machine 


```shell
sudo apt-get update

sudo apt-get install -y --no-install-recommends \
    apt-transport-https \
    ca-certificates \
    curl \
    gnupg-agent \
    software-properties-common

sudo add-apt-repository "deb [arch=amd64] https://download.docker.com/linux/debian $(lsb_release -cs) stable"

sudo apt-get update

sudo apt-get install docker-ce docker-ce-cli containerd.io
```

## Install docker compose

sudo curl -L "https://github.com/docker/compose/releases/download/1.28.2/docker-compose-$(uname -s)-$(uname -m)" -o /usr/local/bin/docker-compose
sudo chmod +x /usr/local/bin/docker-compose


## Copy the docker-compose file from the server-files folder
Note: currently this is setup for my private docker repo. You'll need to build the api image, place it in your own docker hub repo and update the configuration.  

## Copy database.env from the server-files folder to the home/username folder
Note: Update to the password and username you're using for the DB

## Copy the data folder across from server-files folder to the root of your user folder in WinSCP
Note: run this first - sudo chown username /home/username/data/nginx




## Setup let's encrypt certificate
```shell
curl -L https://raw.githubusercontent.com/wmnnd/nginx-certbot/master/init-letsencrypt.sh > init-letsencrypt.sh
chmod +x init-letsencrypt.sh
sudo ./init-letsencrypt.sh
Update the init-letsencrypt.sh file. Change the line 'init-letsencrypt.sh' domains=(example.org www.example.org) to domains=(yourdomain.com www.yourdomain.com)
```

## Login to your docker account 
sudo docker login

## Run docker compose
sudo docker-compose pull && sudo docker-compose up


