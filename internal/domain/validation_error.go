package domain

type ErrorType int

const (
	InvalidRequest ErrorType = iota
	Unexpected
)

type ValidationError struct {
	ErrCode ErrorType
	Error   []error
}
