package domain

import (
	"time"

	"github.com/google/uuid"
	"github.com/lestrrat-go/jwx/jwt"
	"github.com/omini-services/omini-opme-be/internal/constants"
	"gorm.io/gorm"
)

type Auditable struct {
	CreatedBy   uuid.UUID `gorm:"type:uuid;not null;column:CreatedBy"`
	CreatedAt   time.Time `gorm:"type:timestamptz;not null;column:CreatedAt"`
	UpdatedBy   uuid.UUID `gorm:"type:uuid;not null;column:UpdatedBy"`
	UpdatedAt   time.Time `gorm:"type:timestamptz;not null;column:UpdatedAt"`
	IsDeleted   bool      `gorm:"type:bool;not null;column:IsDeleted;default:false"`
	IsDeletedAt time.Time `gorm:"type:timestamptz;null;column:IsDeletedAt"`
	IsDeletedBy uuid.UUID `gorm:"type:uuid;null;column:IsDeletedBy"`
}

func (a *Auditable) BeforeCreate(tx *gorm.DB) error {
	token, _ := tx.Get(constants.JwtKey.String())

	tokenValue := token.(jwt.Token)
	user, _ := uuid.Parse(tokenValue.JwtID())
	a.CreatedBy = user

	return nil
}
