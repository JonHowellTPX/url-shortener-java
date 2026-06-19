# Build stage
FROM maven:3.9.9-eclipse-temurin-17 AS build
WORKDIR /app

COPY pom.xml ./
COPY src ./src

RUN mvn -B package -DskipTests

# Runtime stage
FROM eclipse-temurin:17-jre-jammy
WORKDIR /app

RUN addgroup --system appgroup && adduser --system --ingroup appgroup appuser
RUN mkdir -p /data && chown appuser:appgroup /data
VOLUME ["/data"]

COPY --from=build /app/target/url-shortener-0.0.1-SNAPSHOT.jar ./app.jar

ENV DATABASE_PATH=/data/urlshortener.db
ENV SERVER_PORT=8080

USER appuser
EXPOSE 8080
ENTRYPOINT ["java", "-jar", "/app/app.jar"]
