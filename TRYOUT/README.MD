# TRYOUT

```bash
$ docker build -f .\TRYOUT\Dockcerfile --force-rm -t tryout --target base .\TRYOUT

$ docker network create tryout-network

$ docker run -d --name tryout-con --network tryout-network -p 5000:80 tryout
```