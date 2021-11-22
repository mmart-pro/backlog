FROM node:16

WORKDIR /app

COPY package.json .
COPY yarn.lock .

RUN yarn install
RUN yarn build
COPY . .

ENV HOST 0.0.0.0
EXPOSE 3000

CMD [ "yarn", "start" ]
