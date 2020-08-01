# ywa-tracc

This application allows at-home yogis who follow the Yoga with Adriene ("YWA")
YouTube channel to easily browse and watch videos, and more importantly log
history of videos they have viewed for keeping track of how much yoga they've
been doing. Thus, "ywa-tracc" is short for "Yoga-with-Adriene-tracker".

**Note:** I have no affiliation with Yoga with Adriene, I'm just a fan who made
this as a pet project. I assume that she wouldn't care, since I am not making
any money off of this, and the only impact it could have on her channel
is increased viewership (which is good, right?).

## Inspiration

I have found that I can't easily build certain habits without some sort of
gamified/recorded "streak". For example, I use timer app for meditation which
gives "awards" for every consecutive 10 days of use, and shows bar charts
with how many minutes per day, week, month, etc. to allow you to track your
progress. While the app I use has a separate timer for yoga, I usually watch
YWA videos rather than set a timer and do yoga without guidance. Rather than
start a video and simultaneously start a timer on my phone, I thought it would
be more useful to allow specific videos to be recorded, so that's kind of how I
arrived at the idea for this web application.

## Workflow of Use 

A user of the site can view videos without signing in, but must create an
account to log progress and view stats. User visits the Select page and chooses
a video. The site navigates to the Watch page, with the videoID as a parameter.
Concurrently, the site opens a new tab to the YouTube page corresponding to the
chosen video (unless the popup was blocked, in which case User can click a link
within the Watch page to open the video). Once the yoga session is over, there
is a hyperlink on the Watch page that will record the user/video combination in
the database. It will also redirect the user to the Stats page, which shows
their watch history in the form of a table.

## What Went Well
- Use of a LastRefresh table in the database to minimize API calls. As more videos get posted, my listing in the DB will become stale, but I don't want to waste API calls since there are limits. I implemented a LastRefresh table, which is initially empty. If a user visits the Select page when this table is empty, the server will pull all YWA videos from the YouTubeAPI and insert the current datetime stamp into LastRefresh. Going forward, visiting the Select page causes the app to check LastRefresh for a date that is earlier than today, and if so, pulls only the most recent 50 videos from the channel and inserts the ones that don't exist already.
- The "pull videos for a given channel" API route does not have the duration of the video readily available; that's found via a different call to the API. To avoid making a massive number of API calls to populate these, I leave them as 0 until a video is requested to be recorded. At that point, if the video has 0 duration, the API call is made and the duration is written to the database. This ensures that each video gets 1 API call to find its duration, and they are made on an as-needed basis. 

## Room for Improvement
- Some places felt like I wasn't using best practices (using ViewBag on some of the views, placing all actions in the HomeController)
- Unsure of how well crafted the YouTubeCaller static class is... its current iteration is more DRY than it was, but feels like more logic can be broken out into more methods to make things cleaner

## Short-to-Medium Term Enhancements
- Change Stats page to include data visualizations aggregated at the day/week/month/year level rather than just a flat table of my all-time view history
- Add video tagging functionality so that tags for various purposes (videos for particular muscle groups, intensity levels, durations \[provided enough durations are populated\], series, inversions etc). Could even get granular enough to tag the individual poses (e.g. show me all videos that involve bakasana)

## Longer Term Enhancements
- Implement actual email registration confirmation
  - Also, having an email service would allow *actual* password reset. Currently there is no way to retrieve a lost password
- Incorporate logging in via Google or Facebook
- Deploy to AWS or Azure so that data can be accessed on other devices than just my local machine/DB
