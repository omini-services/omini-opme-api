package domain

import (
	"time"

	"github.com/google/uuid"
)

type User struct {
	ID        uuid.UUID `gorm:"type:uuid;primaryKey;column:ID"`
	FullName  string    `gorm:"type:varchar(400);not null;column:Name"`
	Email     string    `gorm:"type:varchar(400);not null;column:Email"`
	Role      int16     `gorm:"type:uint8;not null;column:Role"`
	CreatedAt time.Time `gorm:"type:timestamptz;not null;column:CreatedAt"`
}
