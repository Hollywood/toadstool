workflow "Deploy on Heroku" {
  on = "push"
  resolves = ["verify"]
}

# Login
action "login" {
  uses = "docker://superbbears/heroku:1.0.0"
  args = "container:login"
  secrets = ["HEROKU_API_KEY"]
}

# Push
action "push" {
  needs = ["login"]
  uses = "docker://superbbears/heroku:1.0.0"
  args = "container:push --app heroku-example-octodex web"
  secrets = ["HEROKU_API_KEY"]
  env = {
    HEROKU_APP = "heroku-example-octodex"
  }
}

# Release
action "release" {
  needs = ["push"]
  uses = "docker://superbbears/heroku:1.0.0"
  args = "container:release --app heroku-example-octodex web"
  secrets = ["HEROKU_API_KEY"]
  env = {
    HEROKU_APP = "heroku-example-octodex"
  }
}

# Verify
action "verify" {
  needs = ["release"]
  uses = "docker://superbbears/heroku:1.0.0"
  args = "apps:info heroku-example-octodex"
  secrets = ["HEROKU_API_KEY"]
}
