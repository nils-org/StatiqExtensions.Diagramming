# Container hosted Kroki

Have a look at https://docs.kroki.io/kroki/setup/use-docker-or-podman/

Short version:

```bash
docker run -p8000:8000 yuzutech/kroki
#or
podman run -p8000:8000 yuzutech/kroki
```

For more services ("bare" kroki, mermaid, bpmn, and excalidraw):

```bash
docker-compose -f kroki.yml up
#or
podman-compose -f kroki.yml up
```

When the container is up, http://localhost:8000 is your endpoint to use!
