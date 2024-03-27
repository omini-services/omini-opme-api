package plugins

import (
	"github.com/lestrrat-go/jwx/jwt"
	"github.com/omini-services/omini-opme-be/internal/constants"
	"gorm.io/gorm"
)

type AuditableCallback struct{}

var (
	// Register the plugin
	_ gorm.Plugin = &AuditableCallback{}
)

const (
	//spankey = "auditable-entity"

	_eventBeforeQuery  = "auditable-entity-event:before_query"
	_eventBeforeCreate = "gorm-tracing-event:before_create"
	_eventBeforeUpdate = "gorm-tracing-event:after_update"
	_eventBeforeDelete = "gorm-tracing-event:before_delete"
	// _eventAfterUpdate  = "gorm-tracing-event:after_update"
	// _eventBeforeQuery  = "gorm-tracing-event:before_query"
	// _eventAfterQuery   = "gorm-tracing-event:after_query"
	// _eventBeforeDelete = "gorm-tracing-event:before_delete"
	// _eventAfterDelete  = "gorm-tracing-event:after_delete"
	// _eventBeforeRow    = "gorm-tracing-event:before_row"
	// _eventAfterRow     = "gorm-tracing-event:after_row"
	// _eventBeforeRaw    = "gorm-tracing-event:before_raw"
	// _eventAfterRaw     = "gorm-tracing-event:after_raw"

	//_opQuery = "query"
)

func NewAuditableEntity() gorm.Plugin {
	return &AuditableCallback{}
}

func (i *AuditableCallback) Name() string {
	return "AuditableEntity"
}

func (i *AuditableCallback) Initialize(db *gorm.DB) error {
	for _, e := range []error{
		db.Callback().Query().Before("gorm:query").Register(_eventBeforeCreate, beforeQuery),
		db.Callback().Create().Before("gorm:create").Register(_eventBeforeCreate, beforeCreate),
		db.Callback().Update().Before("gorm:update").Register(_eventBeforeUpdate, beforeUpdate),
		db.Callback().Delete().Before("gorm:delete").Register(_eventBeforeDelete, beforeDelete),
	} {
		if e != nil {
			return e
		}
	}

	return nil
}

func beforeQuery(db *gorm.DB) {
	db.Where("IsDeletedAt IS NULL")
}

func beforeCreate(db *gorm.DB) {
	token, _ := db.Get(constants.JwtKey.String())

	tokenValue := token.(jwt.Token)

	field := db.Statement.Schema.LookUpField("CreatedBy")
	field.Set(db.Statement.Context, db.Statement.ReflectValue, tokenValue.JwtID())
	db.Where("IsDeletedAt IS NULL")
}

func beforeUpdate(db *gorm.DB) {
	db.Where("IsDeletedAt IS NULL")
}

func beforeDelete(db *gorm.DB) {
	db.Where("IsDeletedAt IS NULL")
}
