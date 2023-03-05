# TRYOUT.UI

```bash
$ docker build -t tryout.ui .

$ docker network create tryout-network

$ docker run -d --name tryout.ui-con --network tryout-network -p 4200:80 tryout.ui
```