.DEFAULT_GOAL := help

createmigration:
	migrate create -ext=sql -dir=sql/migrations -seq $(name)
	
migrateup:
	migrate -path=sql/migrations -database "cockroach://dk:M5RX0zreO7Pz-Su3bZPVmw@peppy-orc-7035.g8z.gcp-us-east1.cockroachlabs.cloud:26257/defaultdb?sslmode=verify-full" -verbose up

migratedown:
	migrate -path=sql/migrations -database "cockroach://dk:M5RX0zreO7Pz-Su3bZPVmw@peppy-orc-7035.g8z.gcp-us-east1.cockroachlabs.cloud:26257/defaultdb?sslmode=verify-full" -verbose down

run:
	go run -ldflags="-X 'main.version=v1.0.0' -X 'main.build=$(shell date +'%Y-%m-%d %H:%M:%S')'" cmd/main.go

wire:
	cd cmd/api && wire

.PHONY: migrate

help:
	@echo "Usage: make <target>"
	@echo ""
	@echo "Targets:"
	@echo "  createmigration Create a new migration (name=<value>)"
	@echo "  migrateup Run migrate up"
	@echo "  migratedown Run migrate down"
	@echo "  run Run application"
	@echo "  help Display this help message"