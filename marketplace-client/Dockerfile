# Use a specific Node.js version base image
FROM node:20-alpine

# Set the working directory inside the container
WORKDIR /app

# Copy package.json and package-lock.json (or npm-shrinkwrap.json)
COPY package*.json ./

# Install dependencies
RUN npm install

# Copy the rest of the application code
COPY . .

# Expose the port Vite runs on (default is 5173)
EXPOSE 5173

# Command to run the development server
CMD ["npm", "run", "dev"]