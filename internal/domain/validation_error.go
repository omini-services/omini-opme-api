package domain

type ErrorType int

const (
	InvalidRequest ErrorType = iota
	Unexpected
	InvalidId
)

type ValidationError struct {
	ErrCode ErrorType
	Error   []error
}
