#To allow a port
sudo ufw allow 5000
sudo ufw enable

#if its ufw is not installed is probably iptables
sudo iptables -A INPUT -m state --state NEW -m tcp -p tcp --dport 3306 -j ACCEPT

# run nginx
docker run --name nginx?? -p 80:80 -d nginx
docker run --name nginx-server -p 8080:8080 -d nginx
#stop
docker stop nginx

#docker login
docker login --username=tainguyen1501xxx --email=xxxx.xxx@outlook.com
Password: yourpass
Login Succeeded

#docker login
docker push maryatdocker/docker-whale
The push refers to a repository [maryatdocker/docker-whale] (len: 1)
7d9495d03763: Image already exists
c81071adeeb5: Image successfully pushed

#redis edit config and restart
vi /etc/redis/redis.conf 
sudo service redis-server restart
redis-cli -h 45.119.87.58 -p 6379

SET test abc
GET test


#docker buid asp net core and run
docker build -t timecommerapiadmin .
docker run -d -p 6002:80 --name timecommerapiadmin timecommerapiadmin

#POSTGRES  https://stackoverflow.com/questions/37694987/connecting-to-postgresql-in-a-docker-container-from-outside
docker run --name sql-postgres -e POSTGRES_PASSWORD=mysecretpassword -d -p 5432:5432 postgres
#run POSTGRES sql test
docker exec -it 05b3a3471f6f bash
root@05b3a3471f6f:/# psql -U postgres
postgres-# CREATE DATABASE mytest;
postgres-# \q

#MONGODB https://stackoverflow.com/questions/34559557/how-to-enable-authentication-on-mongodb-through-docker
#mongo guide https://www.code4it.dev/blog/run-mongodb-on-docker
docker run --name sql-mongodb -p 27017:27017 -d mongo

#run mongo with auth
docker run --name sql-mongodb-auth -p 27017:27017 -e MONGODB_INITDB_ROOT_USERNAME=mongo-admin -e MONGODB_INITDB_ROOT_PASSWORD=P@$$WW0rd -d mongo


docker run -d -p 27017:27017 --name test-mongo -v mongo-data:/data/db -e MONGODB_INITDB_ROOT_USERNAME=sample-db-user -e MONGODB_INITDB_ROOT_PASSWORD=sample-password mongo:latest


#MONGODB INSTALL WITHOUT DOCKER
#https://techviewleo.com/install-mongodb-on-ubuntu-linux/
sudo systemctl restart mongod
mongosh
use admin
db.createUser(
    {
        user: "admin",
        pwd: "P@$$VV0rD!@#",
        roles: [ { role: "userAdminAnyDatabase", db: "admin" } ]
    }
)

use admin;
db.grantRolesToUser('admin', [{ role: 'root', db: 'admin' }])
db.changeUserPassword("admin", "NEWPASSWORD")
exit;

mongo --port 27017 -u admin -p PA$$VV0RD  --authenticationDatabase admin
mongosh admin --username root --password PA$$VV0RD
#if not permeission
mongosh -u admin

# start mongodb if failed
service mongod start


# XRDP REMOTE LINUX-------------------------------------------------
sudo apt update

# Install the main xrdp package.
# It will bring in all its dependencies
# -------------------------------------
sudo apt install xrdp

# Although xrdp is already started,
# make sure its restarted if the machine reboots
# -------------------------------------------
sudo systemctl enable xrdp

# Open up the RDP port 3389 in the firewall
# -----------------------------------------
sudo ufw allow from any to any port 3389 proto tcp 

# show the IP Address of the machine
# ---------------------------------
ip address
# END XRDP REMOTE LINUX-------------------------------------------------




#DOCKER config nginx DOMAIN ETC--------------------https://www.theserverside.com/blog/Coffee-Talk-Java-News-Stories-and-Opinions/Docker-Nginx-reverse-proxy-setup-example
docker run --name nginx-config-ok -p 8080:80 -v /etc/nginx/sites-enabled:/etc/nginx/conf.d -v /etc/nginx/certs-enabled:/etc/nginx/certs -v /var/log/nginx:/var/log/nginx nginx
docker run --name nginx?? -p 80:80 -d nginx
docker run --name nginx-server -p 8080:8080 -d nginx

docker exec -it a96db6836a64 bash
clear
curl
curl http://45.119.87.58:8080

nginx -t
nginx -s reload

#mv is backup
mv default.conf default.conf.bak

apt-get update
apt-get install vim


$ docker run --name tmp-nginx-container -d nginx

docker cp a96db6836a64:/etc/nginx/nginx.conf /test/nginx.conf
docker cp /test/nginx.conf a96db6836a64:/etc/nginx/nginx.conf


$ docker rm -f tmp-nginx-container

sudo docker cp /test/default_nginx_config.conf a96db6836a64:/etc/nginx/conf.d
sudo docker cp a96db6836a64:/etc/nginx/conf.d /test/default.conf 

sudo docker exec a96db6836a64 nginx -t
sudo docker exec a96db6836a64 nginx -s reload

option
sudo docker commit nginx-base nginx-proxy

#END DOCKER config nginx----------------



#NGINX WITHOUT DOCKER https://serverspace.io/support/help/nginx-virtual-hosts-on-ubuntu-20-04/
apt install nginx

#END NGINX WITHOUT DOCKER 


#connect redis 
45.119.87.58:6379

#connect postgres 
server=45.119.87.58;port=5432;userid=postgres;password=mysecretpassword;database=Identity-Dev;

#connect mongodb 
mongodb://45.119.87.58:27017


#client connect redis cli
redis-cli -h 45.119.87.58 -p 6379
ping


#LET ENCRYPT
https://www.digitalocean.com/community/tutorials/how-to-secure-nginx-with-let-s-encrypt-on-ubuntu-20-04
sudo certbot --nginx -d timecommerceapi.metisolution.com 
#let encrypt
sudo certbot --nginx -d admin.timecommerce.store
sudo certbot --nginx -d stores.timecommerce.store
sudo certbot certonly --manual -d *.timecommerce.store --agree-tos --manual-public-ip-logging-ok --preferred-challenges dns-01 --server https://acme-v02.api.letsencrypt.org/directory

certbot certonly --manual --preferred-challenges=dns --email nguyentantai.hcmc@gmail.com --server https://acme-v02.api.letsencrypt.org/directory --agree-tos -d '*.timecommerce.store'


#### -d www.example.com

#CONFIG SERVER
sudo ufw status
sudo systemctl restart nginx
OF
sudo service nginx restart
sudo systemctl reload nginx

rm /etc/nginx/sites-available/stores.timecommerce.store 
rm /etc/nginx/sites-enabled/stores.timecommerce.store 
vi /etc/nginx/sites-available/stores.timecommerce.store 

sudo ln -s /etc/nginx/sites-available/stores.timecommerce.store  /etc/nginx/sites-enabled/

sudo rm -s /etc/nginx/sites-enabled/testsite.com.conf 

sudo rm --dir -s /etc/nginx/sites-enabled/testsite.com

admin.timecommerce.store

 
sudo ln -s /etc/nginx/sites-available/stores.timecommerce.store /etc/nginx/sites-enabled/


vi /etc/nginx/sites-available/stores.timecommerce.store 

sudo nginx -t
sudo systemctl reload nginx


sudo ln -s /etc/nginx/sites-available/site1 /etc/nginx/sites-available/


server {
       listen 80;
       listen [::]:80;

       server_name timeidentity.metisolution.com www.timeidentity.metisolution.com;

       location / {
               proxy_pass http://localhost:9004;
       }
}


#run node js deploy
npm install pm2@latest -g
sudo pm2 install typescript
sudo pm2 install @types/node

pm2 start app.js --port 1520
pm2 start --no-daemon app.js




upstream  tchat {
	ip_hash;
	#server 127.0.0.1:3001 weight=1;
	server 127.0.0.1:7123;
}

server {
	server_name tchat.metisolution.com www.tchat.metisolution.com;

	location / {
			proxy_pass tchat;

			proxy_http_version 1.1;
			proxy_set_header Upgrade $http_upgrade;
			proxy_set_header Connection $connection_upgrade;
	}
}



scp -r ./build/* root@45.119.87.58:/var/www/sites/admintimecommerce

  root /var/www/sites/admintimecommerce;
  index index.html index.htm index.nginx-debian.html;
  
  #server
  root@45.119.87.58
  v6F7Et2F6X2h
  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
  
  
  check disk space
  df -h --total
  du -sxh * --total
  
  
  #docker clear disk
  docker system prune
  
  #If that's not enough, do:

docker system prune -a

#If you haven't got what you expected, try the following

docker volume prune

#docker backup
	#1 Exec container 
	docker exec -it 42b9806f640a sh
	cd wwwroot/test/content/

	#2 Exec copy all folders from root to docker
	docker cp test/. 42b9806f640a:/app/wwwroot/test/content/test
	docker cp backup/taitest/. 42b9806f640a:/app/wwwroot/test/content
	
	#3 Exec copy all folders from docker to root 
	docker cp 42b9806f640a:/app/wwwroot/test/content/. backup/taitest

#git
git reset --hard HEAD~1
git reset --soft HEAD~1


#trick multiple languages
ls -la ~/ | more
vi ~/.bashrc
#add for asp multiple languages
export DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1









upstream  store {
        ip_hash;
        server 127.0.0.1:3001;
}

server {

        server_name timecommerceapi.metisolution.com www.timecommerceapi.metisolution.com;

        location / {
			proxy_pass http://timecommerceapi;
	
			proxy_http_version 1.1;
			proxy_set_header   Upgrade $http_upgrade;
			proxy_set_header   Connection keep-alive;
			proxy_set_header   Host $host;
			proxy_cache_bypass $http_upgrade;
			proxy_set_header   X-Forwarded-For $proxy_add_x_forwarded_for;
			proxy_set_header   X-Forwarded-Proto $scheme;
        }
}


upstream  admintimecommerce {
        ip_hash;
        server 127.0.0.1:6002;
}

server {

        server_name admin.timecommerce.store www.admin.timecommerce.store;

        location / {
			proxy_pass http://admintimecommerce;
	
			proxy_http_version 1.1;
			proxy_set_header   Upgrade $http_upgrade;
			proxy_set_header   Connection keep-alive;
			proxy_set_header   Host $host;
			proxy_cache_bypass $http_upgrade;
			proxy_set_header   X-Forwarded-For $proxy_add_x_forwarded_for;
			proxy_set_header   X-Forwarded-Proto $scheme;
        }
}


1. Backend: Asp net core
2. Frontend: Angular js, Next js, Tailwind css, Bootstrap.
3. Database: MONGODB, Redis.
4. Web server: Nginx.
5. CI/CD: Git, docker compose.



upstream  store {
        ip_hash;
        server 127.0.0.1:3001;
}
server {
                listen 80;
                server_name *.timecommerce.store;
        location / {
                proxy_pass http://store;
                proxy_http_version 1.1;
                proxy_set_header   Host $host;
                proxy_cache_bypass $http_upgrade;
                proxy_set_header   X-Forwarded-For $proxy_add_x_forwarded_for;
                proxy_set_header   X-Forwarded-Proto $scheme;
                proxy_pass_request_headers      on;
        }
}


#DOT NET Installsudo apt remove 'dotnet*' 'aspnet*' 'netstandard*'
sudo rm /etc/apt/sources.list.d/microsoft-prod.list
sudo apt update
sudo apt install dotnet-sdk-7.0

#https://learn.microsoft.com/en-us/aspnet/core/security/docker-compose-https?view=aspnetcore-7.0
dotnet dev-certs https -ep ${HOME}/.aspnet/https/aspnetapp.pfx -p P@$$VV0rD
dotnet dev-certs https --trust

dotnet dev-certs https --clean
dotnet dev-certs https -ep ${HOME}/.aspnet/https/aspnetappcert.pfx -p Pass123456 
dotnet dev-certs https --trust
#add to docker
  ports:
      - "6001:443"
      - "6003:80"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ASPNETCORE_URLS=https://+:443;http://+:80
      - ASPNETCORE_Kestrel__Certificates__Default__Password=Pass123456
      - ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetappcert.pfx


#find file
find / -type f -name  aspnetapp.pfx
