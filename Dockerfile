FROM golang:latest as builder
ARG API_VERSION

WORKDIR /app

COPY . .

#CGO_ENABLED=0

RUN GOOS=linux CGO_ENABLED=0 go build -ldflags="-w -s -X 'main.version=${API_VERSION}' -X 'main.build=$(date)'" -o server ./cmd/server 

FROM scratch

WORKDIR /app
COPY --from=builder /app/server .

EXPOSE 8000

ENTRYPOINT ["./server"]

