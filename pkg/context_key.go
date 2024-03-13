package pkg

type ContextKey string

func (c ContextKey) String() string {
	return "mypackage context key " + string(c)
}
