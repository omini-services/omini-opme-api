.DEFAULT_GOAL := help

createmigration:
	migrate create -ext=sql -dir=sql/migrations -seq $(name)
	
migrateup:
	migrate -path=sql/migrations -database "cockroach://dk:M5RX0zreO7Pz-Su3bZPVmw@peppy-orc-7035.g8z.gcp-us-east1.cockroachlabs.cloud:26257/defaultdb?sslmode=verify-full" -verbose up

migratedown:
	migrate -path=sql/migrations -database "cockroach://dk:M5RX0zreO7Pz-Su3bZPVmw@peppy-orc-7035.g8z.gcp-us-east1.cockroachlabs.cloud:26257/defaultdb?sslmode=verify-full" -verbose down

run:
	go run cmd/server/main.go cmd/server/wire_gen.go

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