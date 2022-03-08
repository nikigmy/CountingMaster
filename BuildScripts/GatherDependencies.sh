# go into right dir
cd ../Assets/Dependencies
# download dependencies
curl -sJLH 'Authorization:token '"$GIHUB_TOKEN"'' "https://api.github.com/repos/nikigmy/LibratyTest/releases/latest" \
| grep 'releases/assets' \
| cut -d '"' -f 4 \
| xargs -n 1 curl -JL -O -H 'Authorization:token '"$GIHUB_TOKEN"'' -H "Accept:application/octet-stream" 