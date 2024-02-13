package pkg

func Contains(arr []string, value string) bool {
	for _, s := range arr {
		if s == value {
			return true
		}
	}

	return false
}
