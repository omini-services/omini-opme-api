package configs

import (
	"fmt"
	"os"
)

var AuthorizedRoutes = []string{
	"/api/health",
}

type conf struct {
	DBConnectionString string `mapstructure:"DB_CONNECTION_STRING"`
	WebServerPort      string `mapstructure:"WEB_SERVER_PORT"`
}

func LoadConfig(path string) (*conf, error) {
	cfg := &conf{
		DBConnectionString: os.Getenv("DB_CONNECTION_STRING"),
		WebServerPort:      fmt.Sprintf(":%s", os.Getenv("WEB_SERVER_PORT")),
	}

	// viper.AddConfigPath(path)
	// viper.SetConfigFile(".env")
	// viper.SetConfigType("env")
	// viper.AutomaticEnv()
	// err := viper.ReadInConfig()
	// if err != nil {
	// 	if viper.GetString("DB_CONNECTION_STRING") == "" {
	// 		viper.Set("DB_CONNECTION_STRING", os.Getenv("DB_CONNECTION_STRING"))
	// 	}
	// }
	// err = viper.Unmarshal(&cfg)
	// if err != nil {
	// 	panic(err)
	// }
	return cfg, nil
}
