docker build -t critical-hit-image .

docker tag critical-hit-image registry.heroku.com/critical-hit-products-image/web

docker push registry.heroku.com/critical-hit-products-image/web

heroku container:release web -a critical-hit-image