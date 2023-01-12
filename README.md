# D-real social app

## Ideology

D-real social app is written in .NET for an Information Systems class at the Faculty of Computer and Information Science University of Ljubljana. <br><br>
The idea behind the app is to create a new social app that will not sell data and have an algorithmic content prediction. We want to create a social app where we will see posts from our real friends and not only conspiracy theory posts.

## Description of operation

First, we register in the application (Google or username and password) or log in. Upon registration, the application asks us to confirm the email.
The app then redirects us to the "Feed" page where we can start posting. At the top of the page, we have a search bar where you can search for your friends. On a friend's page, we can see his posts, comment on them or add him as a friend.
If we click on our email in the upper right corner, our account settings will appear.

## Description of tasks

**Tilen:**

- Project init
- Docker encapsulation
- Authentication and authorization
- Feed page

**Vid:**

- User profile page

## Development

### Running

1. `docker-compose up`
2. Open it on http://localhost:3000
3. Change code, save and the browser should hot reload

### Fresh build

`docker-compose down -v && docker-compose build --no-cache && docker-compose up`

## Database

![Databe](https://raw.githubusercontent.com/KomelT/D-real-social-app/main/images/db.png)

Glavne table v bazi so Post, Feed, Connection, Comment.

Post:
  tabela hrani objavo(text ali photo), userja, ki je objavil in timestamp objave.
Feed:
  hrani objave, userja, ki objavi in timestamp objave.
Connection:
  povezuje dva userja med sabo, da lahko vidita objave en drugega.
Comment:
  vsebuje userja, post na katerega pišemo comment in vsebino commentarja
User:
  vsebuje klasične podatne o userju(id, ime...), email, hash passworda + dodatne stari za autetikacijo

## Authors:

Tilen Komel 63210153 \
Vid Marolt 63210201
