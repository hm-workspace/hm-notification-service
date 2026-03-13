# hm-notification-service

Independent microservice repository for Hospital Management.

## Local run

`ash
dotnet restore
dotnet build
dotnet run --project src/NotificationService.Api/NotificationService.Api.csproj
`

## Docker

`ash
docker build -t hm-notification-service:local .
docker run -p 8088:8080 hm-notification-service:local
`

## GitHub setup later

`ash
git branch -M main
git remote add origin <your-github-repo-url>
git add .
git commit -m "Initial scaffold"
git push -u origin main
`
