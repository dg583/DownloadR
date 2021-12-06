# DownloadR - A simple Download-Manager [alpha]

Gives you the possibility to load files in parallel from different sources
### Commandline-Sample
```
download -v|--verbose --parallel-downloads 9 -f|--file "C:\path\to\myFile.yaml"
```

## Sample yaml file
```
config:
 parallel_downloads: 3
 download_dir: ./downloads/linux-images
downloads:
 - url: https://releases.ubuntu.com/20.04.2/ubuntu-20.04.2-live-server-amd64.iso
   file: ubuntu-20.04.2-live-server-amd64.iso
   sha256: d1f2bf834bbe9bb43faf16f9be992a6f3935e65be0edece1dee2aa6eb1767423
   overwrite: true
 - url: https://cdimage.ubuntu.com/releases/20.10/release/ubuntu-20.10-live-server-arm64.iso
   file: ubuntu-20.10-live-server-arm64.iso
   overwrite: false

```
## TODO
- Dryrun with command --dryrun
- Hash-Verification
- Logging to file
