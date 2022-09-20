#!/bin/sh

minikube start
minikube addons enable metrics-server
minikube addons enable ingress
minikube addons enable dashboard