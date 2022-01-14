FROM nginx
COPY nginx.conf /etc/nginx/nginx.conf
RUN mkdir -p /www
ADD build /www/

EXPOSE 80