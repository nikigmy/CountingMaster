curl -sJLH "Authorization:token $1" "https://api.github.com/repos/TurnHero/TurnHero.Game.Models/releases/latest" \
| grep 'releases/assets' \
| cut -d '"' -f 4 \
| xargs -n 1 curl -sJL -O -H "Authorization:token $1" -H "Accept:application/octet-stream" 