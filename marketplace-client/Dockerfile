# --- Stage 1: Build the application ---
FROM node:20-alpine AS build
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
RUN npm run build

# --- Stage 2: Serve the application with Nginx ---
FROM nginx:alpine
# Copy the built assets from the 'build' stage to the Nginx serving directory
COPY --from=build /app/dist /usr/share/nginx/html
# Expose port 80 to the host machine
EXPOSE 80
# Command to start Nginx and serve the static files
CMD ["nginx", "-g", "daemon off;"]