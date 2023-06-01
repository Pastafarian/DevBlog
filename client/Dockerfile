# Stage 1: Compile and Build angular codebase

# Use official node image as the base image
FROM node:20.2.0-alpine3.17 AS build

# Set the working directory
WORKDIR /usr/local/app

# Add the source code to app
COPY ./ /usr/local/app/

# Install all the dependencies
RUN npm install

# Generate the build of the application
RUN npm run build-prod


# Stage 2: Serve app with nginx server

# Use official nginx image as the base image
FROM nginx:latest
COPY nginx.conf /etc/nginx/nginx.conf
# Copy the build output to replace the default nginx contents.
COPY --from=build /usr/local/app/dist/client /usr/share/nginx/html

# Expose port 80 and 443
EXPOSE 80
EXPOSE 443
