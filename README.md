[![Review Assignment Due Date](https://classroom.github.com/assets/deadline-readme-button-24ddc0f5d75046c5622901739e7c5dd533143b0c8e959d652212380cedb1ea36.svg)](https://classroom.github.com/a/CibnTZFQ)

# Project 2 Report

Read the [project 2
specification](https://github.com/COMP30019/Project-2-Specification) for
details on what needs to be covered here. You may modify this template as you see fit, but please
keep the same general structure and headings.

Remember that you must also continue to maintain the Game Design Document (GDD)
in the `GDD.md` file (as discussed in the specification). We've provided a
placeholder for it [here](GDD.md).

## Table of Contents

* [Evaluation Plan](#evaluation-plan)
* [Evaluation Report](#evaluation-report)
* [Shaders and Special Effects](#shaders-and-special-effects)
* [Summary of Contributions](#summary-of-contributions)
* [References and External Resources](#references-and-external-resources)


## Evaluation Plan

### Evaluation Techniques:

- **Querying technique:** Semi-structured and structured Closed Questions (see Questionnaires)
- **Observational technique:** Cooperative Evaluation and Post-task walkthroughs.
- **Playtesting:** Engage participants to play the game and gather qualitative feedback on their experience.
- **Usability Testing:** Identify any interface or navigation issues players might encounter.
- **Questionnaires:** Use post-play surveys to gather player feedback on various aspects like gameplay mechanics, faith points system, and overall experience.


### Tasks for Participants:

1. Play the game until the dawn (5 minutes) multiple times to experience the content.
2. Engage with different random skills and relics to evaluate their impact on faith points.
3. Interact with in-game UI and settings to test usability.
4. Answer several question after the whole process

### Participants:

- **Recruitment:** Recruit participants via social media announcements, game forums, and through our company website.
- **Qualifying Criteria:** Participants should have experience with survival games, aged between 16-35, and have a mix of casual and core gamers.

### Data Collection:

- **Data Type:** 
    - Gameplay duration and success rates.
    - Frequency of faith point depletion to 0.
    - Most and least used random skills.
    - Qualitative feedback on player experience.
- **Collection Method:** Use built-in game analytics for quantitative data and post-play surveys for qualitative data.
- **Tools:** Game analytics platform (e.g., Unity Analytics) and online survey tools (e.g., SurveyMonkey).

### Data Analysis:

- **Quantitative Analysis:** Use statistical tools to identify patterns in gameplay success, skill usage, and faith point management.
- **Qualitative Analysis:** Conduct thematic analysis of player feedback to understand game strengths, weaknesses, and areas of improvement.
- **Metrics:** 
    - Player retention rate.
    - Average faith points at the end of a game.
    - Frequency of skill use.
    - Overall player satisfaction scores from surveys.

### Timeline:

- **Evaluation Duration:** 2 weeks.
- **Data Analysis:** 1 week post-evaluation.
- **Game Modifications:** Implement changes in the subsequent month based on the feedback received.

### Responsibilities:

- **Project Manager (One of the team members.):** Has the fun job of overseeing the whole shebang, ensuring we don't fall behind our wildly optimistic timelines.
- **Game Developers (One of the team members.):** Those magicians who sprinkle some code dust, implement mysterious tracking tools, and wave their coding wand to make post-evaluation magic changes.
- **Data Analyst (One of the team members):** Digs deep into the avalanche of data, seeking the nuggets of wisdom and then, in a grand ceremony, presents the treasures (findings).
- **UX/UI Team (One of the team members.):** Fixes the "oh-so-obvious" interface and navigation hiccups that mere mortals might overlook.
- **Equal Contribution :** We hold regular team meetings, to make sure everyone's working at the same pace.

## Evaluation Report

In our recent game testing session, we invited a total of twelve participants to engage with our game. They provided us with an ample number of samples (the number of people for querying and one observational evaluation technique both exceeded 5) and **12 valid questionnaire results**. All participants were within our target demographic, aged between **20 and 26** years old. The group consisted of eight males and four females, and they were unaware of the game’s content prior to the session. This demographic is particularly significant for our game, as it aligns perfectly with our intended audience.

We employed Cooperative Evaluation and Post-task Walkthroughs as our primary methods of observation during the game testing. This approach allowed us to interact with the participants throughout the gaming session, observing their behavior, and gathering real-time feedback. Following the gaming session, participants were asked to share their thoughts and complete a questionnaire, the details of which can be found in the provided PDF on our GitHub repository, named `“Questionnaire_for_Game_Evaluation.pdf”`.

From our analysis of the gathered data, we were able to draw several key conclusions:

1. **Demographic Relevance**: Given that all participants fell within the 20-26 age range, and were students, we acknowledge that while the age range is narrow, it is representative of our target audience. This demographic relevance adds significant value to our findings.

2. **Game Attraction**: Our game received an average rating of **7.58 out of 10**. This score is indicative of a positive reception and suggests that our game holds a certain level of attraction to players.

3. **Positive Feedback**: Several aspects of the game were highlighted positively by the participants. Three of them particularly appreciated the background story and the thematic settings of the game, stating that the game did an excellent job of capturing these elements. Additionally, subtle effects in the UI, as well as the character models, were praised. Almost all subjects were 'POSITIVE' and above in their impression of the game, and a unanimously chosen 5 (Very Easy) for the clarity of the UI.

4. **Areas for Improvement and Action Taken**: 
    - **Lighting**: A significant portion of participants (**10 out of 12**) mentioned that the game’s lighting was too dim. In response to this feedback, we have increased the number of light sources and adjusted the overall brightness in the final version of the game.
    - **Game Objectives**: Some participants (**6 out of 12**) expressed that the game’s objectives were not clear enough. To address this, we introduced a Timer feature, providing players with a clearer sense of direction and motivation to continue playing.

In conclusion, the game testing session provided us with valuable insights into the player experience, highlighting areas of strength and opportunities for improvement. The adjustments made following the session reflect our commitment to creating a game that resonates with our target audience and offers an engaging and enjoyable experience.


## Shaders and Special Effects

TODO (due milestone 3) - see specification for details

## Summary of Contributions

| Name             | Contribution |
|------------------|--------------|
| Yuecheng Wang    | 33.3%        |
| Junyan Lai       | 33.3%        |
| Jingxuan Zhang   | 33.3%        |

Among them, Wang completed tasks including the design of the map, character models, and code implementation, as well as parts of the shader work. Lai was responsible for the UI design and setup, enemy AI, and maintenance of the cloud repository. Zhang took charge of establishing the game's core mechanics and implementing the game life cycle. Overall, despite one team member dropping out mid-course (originally we had four members), the three remaining members managed to complete the task on time and with quality, enthusiastically cooperating to construct an outstanding game project.
Specific details can be viewed in the commit history section on GitHub.

## References and External Resources

We modified some open-source models available on the internet (URLs:), and for the implementation of certain specific functions, we may have referred to some YouTube tutorials (URLs: https://www.youtube.com/watch?v=br9YzpiBeIw, https://www.youtube.com/watch?v=cqNBA9Pslg8, https://www.youtube.com/watch?v=vApG8aYD5aI). We utilized a large language model (CHATGPT) for some translation work, and our implementation of some features might also have referred to the content of workshops 4, 7, and 8.
