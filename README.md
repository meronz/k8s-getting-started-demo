# Getting started with ✨Kubernetes and Skaffold✨

### Setup
You will need Minikube and Skaffold. To setup a cluster for this demo, launch:

```shell
minikube start
minikube addons enable metrics-server
minikube addons enable ingress
minikube addons enable dashboard
```

and once it finishes, run:
```shell
kubectl port-forward -n ingress-nginx svc/ingress-nginx-controller 8080:80
```

to expose the application on [http://localhost:8080](http://localhost:8080)

### Build and deploy the application
In the root folder, launch:
```shell
skaffold run
```

### Debug using Visual Studio Code
You need to install the recommended extensions, such as:
- C#
- Kubernetes
- Docker
- Bridge to Kubernetes


Then run one of the two launch configurations:
- **Debug BlazorCharts with Kubernetes**: Launch locally and redirects the traffic to your machine.
- **Attach to Kubernetes**: Attaches the debugger to an instance already running in the cluster.


### Have fun! 🤓