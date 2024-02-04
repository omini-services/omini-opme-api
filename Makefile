createmigration:
	migrate create -ext=sql -dir=sql/migrations -seq init
	
migrateup:
	migrate -path=sql/migrations -database "cockroach://dk:M5RX0zreO7Pz-Su3bZPVmw@peppy-orc-7035.g8z.gcp-us-east1.cockroachlabs.cloud:26257/defaultdb?sslmode=verify-full" -verbose up

migratedown:
	migrate -path=sql/migrations -database "cockroach://dk:M5RX0zreO7Pz-Su3bZPVmw@peppy-orc-7035.g8z.gcp-us-east1.cockroachlabs.cloud:26257/defaultdb?sslmode=verify-full" -verbose down

.PHONY: migrate