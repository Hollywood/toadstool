# GitHub Repository Webhook Listener

## Overview

The functionality contained within this project will create an issue and tag all specified collaborators in a pre-defined repository whenever a specified repository is deleted. This was achieved using GitHub Organization Webhooks, a C# WebAPI project with the third party library OctoKit to make API calls, [ngrok](https://ngrok.com/) for local web hosting, and a powershell script to be ran by the appropriate individuals to insert any sensitive API keys. 

## Installation and Usage
There are a couple of ways to implement this event listener and each method has its value. But before installing the server, we have to configure our GitHub Organization to contain a Webhook that listens for any repository deletion.

## GitHub Configuration

### Creating an Account  
First things first, we need a GitHub account. If you don't already have an account, head over to [GitHub](https://github.com/) and create one. 
You can create one instantly from their landing page and you'll be up and running within seconds.   

![](http://i.imgur.com/iTVWLha.png)

### Creating an Organization
Once logged in, click the avatar for your account in the upper right hand corner of the screen and enter the settings menu.

![](http://i.imgur.com/HvwU4Zo.png)

You'll notice a list of options to the left of your screen once you enter into your account settings. Since we want to create an Organization, click on the **Organizations** link.

![](http://i.imgur.com/BQXX6q3.png)

GitHub provides users with two options in regards to creating Organizations. You can create a conceptual Organization from scratch, or you can turn your actual user account into an Organization. For the purpose of our exercise here, we will be creating a new Organization, not converting our account.

![](http://i.imgur.com/HggsQ8r.png)

GitHub provides its users with a pretty intuitive workflow when it comes to the process of creating an Organization. You pick a name, enter a billing email address, pick a service plan, and then invite any users you wish to be a part of it.

![](http://i.imgur.com/VNrMz94.png) 

## Creating an Organizational Webhook

Once your Organization is created, you'll want to go and create a Webhook that forwards any **delete** events to your service.

On the main page for your Organization, click the settings tab at the top of the screen. 

![](http://i.imgur.com/VHir1Y2.png)

This screen will look similar to the user account settings screen. Click the **Webhooks** link on the left hand side to create your webhook.

![](http://i.imgur.com/7IE7yha.png?1)

The next screen that shows will have a button in the upper right hand corner labeled "Add Webhook". Click that to create a new instance of an Organization Webhook. Your options will depend on how your web service is hosted, the format in which you want to consume your response body, and a box labeled "Secret" to ensure that your connection is secure. For the time being, put **http://localhost:4567** as your Payload URL, we will change this later. 

There are many ways to create a Secret for the purposes of using Webhooks. For instance, if you have Ruby installed on your system the following command will give you a secure value to use:

**ruby -rsecurerandom -e 'puts SecureRandom(20)'**

That result will look similar to:

**80ea0dfd95ee381edad531275d1d86f349ae172f**

Use the calculated value as your secret and make sure it remains secure. There are many different approaches to keeping this under lock and key such as using Environment Variables, a service like as Azure Key Vault, or a script file that populates these values during your CI/CD process. **Under no circumstances should a configuration file containing a plain text secret value be committed to the repository. Include this file in your .gitignore before committing.**

Once all of your values are configured click the option labeled "Let me select individual events" at the bottom of the Webhook creation screen. For the purpose of this exercise we're only going to select the **Repository** option. 

![](http://i.imgur.com/H2jpeGv.png)

Ensure that the **Active** option is selected and click **Add Webhook**. Congrats! You just created an Organizational Webhook!

An Organizational Webhook alone won't satisfy our needs from an authentication standpoint. Since we need to create an issue any time a repository is deleted, we will also need to authenticate ourselves as a user. This requires us to create a personal access token.

Head back to the user settings screen (top right hand corner, click your avatar, and then click Settings in the dropdown menu.)

Scroll to the bottom of the user options list on the left hand side of the page and click **Personal Access Tokens** under Developer Settings.

![](http://i.imgur.com/DlwOkuq.png)

Enter a token description and select the following options:

![](http://i.imgur.com/c3uBRtM.png) 

Once you create your token make sure to store it securely along with your Organization Webhook token. Once you generate this token, you cannot retrieve the value. You can regenerate it, however you will need to remember to update your code if you do.

## Webhook Listener Service
Our Web Service was built on Microsoft's ASP.NET Web API platform using Microsoft's Webhook library to handle our Webhook notifications along with GitHub's OctoKit library to parse the payload and post an issue to a specified repository when a delete occurs. Since I was developing locally, and localhost is not public facing, GitHub's notifications would not be able to make it to my service. For this I used [ngrok](https://ngrok.com/). Ngrok allows you to expose your local server to the internet by giving you a publicly available URI.

If you have a public facing web server the use of ngrok is unnecessary, just use the domain that is currently setup on your webserver.

## Running the Service

If using localhost, add port 4567 to your default website in IIS. Then publish the code to a virtual directory path that you will need to create within IIS. Once the code is published, you will need to run a powershell script (that I can supply to you all) to populate the secret key values in the web.config. If you encounter any issues updating the values you may need to start Powershell in admin mode. Once you do, run the following command before your update command (please note that in the code's current state, this script needs to be ran upon the completion of every publish.)

**set-executionpolicy remotesigned**

Enter Y when prompted and you should be good to go.

### Running ngrok
If you need to run ngrok to get a public facing URI, open your command prompt and navigate to the folder that contains the ngrok executable. Once there, the command to get a public URI is:

**ngrok http 4567**

The command should produce a screen that looks like:

![](http://i.imgur.com/Bt6ADL6.png)

Grab the value that is printed next to the label "Forwarding". Also, please keep in mind that the URI value changes every time you start the ngrok service. You will need to update your Webhook if you stop it and restart it.

Once you have the URI you want to use, head back to GitHub and navigate to the Organization settings screen. Click the **Edit** button on the Webhook you created earlier and paste your URI with the following value after it into the Payload URL textbox:

**::YourURI::/api/webhooks/incoming/github**

This will point the Webhook at your handler endpoint and allow for any messages to fall into the workflow. 

Click **Update Webhook** to save your value and lets start deleting some repositories! (Empty ones, please.)

## Service Results

I created two empty repositories named after some nostalgic video game characters. If either of these get deleted, an issue will be automatically created in the king-koopa repository tagging any specified users. To alter the values of what repository will contain the issues or what users will be tagged, change the values contained in the AppSettings group in the Web.Config file. 

Here's what my Organization currently looks like:

![](http://i.imgur.com/RX2BAIV.png)

Inside the Webhook setting screen, all the way at the bottom a history of your Webhook delivery calls are logged. A successful call (200) looks like:

![](http://i.imgur.com/R0DJ1cv.png)

An error will look like the following and give you the response body. A very useful piece of functionality that GitHub gives its users is the ability to redeliver a Webhook request. This makes debugging code so much more efficient!

![](http://i.imgur.com/cVI3a05.png)

I'm going to go ahead and delete that pesky bob-omb repository and I expect to see an issue created in my king-koopa repository after deletion. This issue should be assigned to me, as well as have me tagged in the body.

![](http://i.imgur.com/9v8wYMs.png)

And there it is! As I mentioned above, if you experience any issues with your Webhooks, the Webhook configuration page has all of the information you need to debug your code without having to spend time re-creating repositories!