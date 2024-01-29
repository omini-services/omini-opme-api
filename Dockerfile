FROM golang:latest as builder

WORKDIR /app

COPY . .

#CGO_ENABLED=0
RUN GOOS=linux CGO_ENABLED=0 go build -o server ./cmd/server 

FROM scratch

WORKDIR /app
COPY --from=builder /app/server .

EXPOSE 8000

ENTRYPOINT ["./server"]

