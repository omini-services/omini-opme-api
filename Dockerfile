FROM golang:latest as builder
ARG API_VERSION

WORKDIR /app
COPY . . 
RUN GOOS=linux CGO_ENABLED=0 go build -ldflags="-w -s -X 'main.version=${API_VERSION}' -X 'main.build=$(date)'" -o server ./cmd/server 

FROM alpine:3.17

WORKDIR /app
COPY ./cmd/server/.env .
COPY --from=builder /app/server .

EXPOSE 80
ENTRYPOINT ["/app/server"]