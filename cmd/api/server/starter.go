package main

type WebServerStarter struct {
	WebServer Server
}

func NewWebServerStarter(webServer Server) *WebServerStarter {
	return &WebServerStarter{
		WebServer: webServer,
	}
}
