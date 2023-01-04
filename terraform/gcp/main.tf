resource "google_cloud_run_service" "default" {
  name     = "cloudrun-terraform-test"
  location = "us-central1"
  project = "cloudrun-teste"
  lifecycle {
    
  }

  template {
    spec {
      containers {
        image = "us-docker.pkg.dev/cloudrun/container/hello"
        ports {
          container_port = 8080
        }
      }
      container_concurrency = 1
      timeout_seconds = 300
      
    }
    
  }

  traffic {
    percent         = 100
    latest_revision = true
  }
}

resource "google_cloud_scheduler_job" "scheduler-test" {
  name = "scheduler-for-running-cloudrun-terraform-test"
  description = "Intantiate Cloud Run for cloudrun-terraform-test"
  time_zone = "America/Sao_Paulo"
  project = "cloudrun-teste"
  schedule = "0 8 * * 1-5"
}
