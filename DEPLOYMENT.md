# Deployment Guide - Fly.io

This guide will help you deploy the Norway Powder Alert application to Fly.io with automatic GitHub deployments.

## Prerequisites

1. A [Fly.io account](https://fly.io/app/sign-up) (free tier available)
2. [flyctl CLI](https://fly.io/docs/hands-on/install-flyctl/) installed
3. Your code pushed to a GitHub repository

## Initial Setup

### 1. Install Fly.io CLI

**macOS/Linux:**
```bash
curl -L https://fly.io/install.sh | sh
```

**Windows (PowerShell):**
```powershell
pwsh -Command "iwr https://fly.io/install.ps1 -useb | iex"
```

### 2. Login to Fly.io

```bash
flyctl auth login
```

This will open your browser for authentication.

### 3. Create and Launch Your App

Navigate to your project directory and launch:

```bash
cd norway-powder-alert
flyctl launch
```

You'll be prompted with several questions:
- **Choose an app name**: Press Enter to use `norway-powder-alert` or choose your own
- **Choose a region**: Select `arn` (Stockholm) - closest to Norway for best performance
- **Would you like to set up a PostgreSQL database?**: No (we don't need a database)
- **Would you like to set up an Upstash Redis database?**: No
- **Would you like to deploy now?**: Yes

The app will build and deploy. This may take a few minutes.

### 4. Verify Deployment

Once deployed, Fly.io will provide you with a URL like:
```
https://norway-powder-alert.fly.dev
```

Visit this URL to see your live application!

## Setup GitHub Actions for Automatic Deployment

### 1. Get Your Fly.io API Token

```bash
flyctl auth token
```

Copy the token that's displayed.

### 2. Add Secret to GitHub

1. Go to your GitHub repository
2. Click **Settings** → **Secrets and variables** → **Actions**
3. Click **New repository secret**
4. Name: `FLY_API_TOKEN`
5. Value: Paste your Fly.io token
6. Click **Add secret**

### 3. Push Your Changes

The GitHub Actions workflow is already configured in `.github/workflows/deploy.yml`.

Now, every time you push to the `main` branch, your app will automatically deploy to Fly.io!

```bash
git add .
git commit -m "Add Fly.io deployment configuration"
git push origin main
```

### 4. Monitor Deployment

Watch the deployment in real-time:
- On GitHub: Go to the **Actions** tab in your repository
- On Fly.io: Run `flyctl logs` in your terminal

## Managing Your Deployment

### View App Status
```bash
flyctl status
```

### View Live Logs
```bash
flyctl logs
```

### Scale Your App
```bash
# View current scaling
flyctl scale show

# Scale to multiple machines (for high traffic)
flyctl scale count 2

# Scale back to 1 machine (for cost savings)
flyctl scale count 1
```

### Update Environment Variables
```bash
flyctl secrets set VARIABLE_NAME=value
```

### SSH into Your Container
```bash
flyctl ssh console
```

## Cost Optimization

Fly.io's free tier includes:
- Up to 3 shared-cpu-1x 256MB VMs
- 160GB outbound data transfer

The app is configured with:
- **auto_stop_machines = true**: Machines stop when idle
- **auto_start_machines = true**: Machines start on request
- **min_machines_running = 0**: No machines running when idle (saves resources)
- **256MB memory**: Minimal footprint

This means:
- ✅ Your app runs on the free tier
- ✅ It stops when not in use (no charges)
- ✅ It starts automatically when someone visits
- ✅ Cold start time: ~2-3 seconds

## Custom Domain (Optional)

To use your own domain:

```bash
# Add certificate for your domain
flyctl certs add yourdomain.com

# Get the DNS records to add
flyctl ips list
```

Then add these DNS records to your domain provider:
- An A record pointing to the IPv4 address
- An AAAA record pointing to the IPv6 address

## Troubleshooting

### App Won't Start
```bash
# Check logs
flyctl logs

# Check app status
flyctl status

# Restart the app
flyctl apps restart norway-powder-alert
```

### Deployment Fails
```bash
# Deploy manually to see detailed errors
flyctl deploy
```

### GitHub Action Fails
- Verify `FLY_API_TOKEN` secret is set correctly
- Check the Actions tab for error messages
- Ensure your fly.toml file is committed

## Monitoring

### Check App Health
```bash
flyctl checks list
```

### View Metrics
```bash
flyctl dashboard
```

This opens the Fly.io dashboard in your browser with detailed metrics.

## Cleanup

To delete the app and all resources:

```bash
flyctl apps destroy norway-powder-alert
```

---

## Support

- [Fly.io Documentation](https://fly.io/docs/)
- [Fly.io Community](https://community.fly.io/)
- [GitHub Actions Documentation](https://docs.github.com/en/actions)

Enjoy your deployed powder tracker! 🎿❄️
