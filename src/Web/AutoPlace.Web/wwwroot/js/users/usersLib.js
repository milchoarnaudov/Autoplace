function setupUserDetails(token, username) {
    function getVotes() {
        fetch(`/api/Votes?username=${username}`)
            .then((response) => {
                return response.json();
            })
            .then((data) => {
                let upVoteElement = document.getElementById("upVotesCount");
                let downVoteElement = document.getElementById("downVotesCount");

                upVoteElement.innerText = data.filter(x => x.voteValue).length;
                downVoteElement.innerText = data.filter(x => !x.voteValue).length;
            });
    }

    document.getElementById("commentSubmitBtn").addEventListener('click', function (e) {
        var content = document.getElementById("commentContent").value;

        fetch(`/api/Comments/`,
            {
                method: "POST",
                headers: {
                    'Content-Type': 'application/json;charset=UTF-8',
                    'RequestVerificationToken': token
                },
                body: JSON.stringify({ CommentedUserUserName: `${username}`, content: content })
            })
            .then((response) => {
                if (response.ok) {
                    location.reload();
                }
            })
    });

    document.getElementById("downVote").addEventListener('click', function (e) {
        changeBtnColors(e.target, false);

        fetch(`/api/Votes/`,
            {
                method: "POST",
                headers: {
                    'Content-Type': 'application/json;charset=UTF-8',
                    'RequestVerificationToken': token
                },
                body: JSON.stringify({ ForUserUserName: `${username}`, VoteValue: false })
            })
            .then((data) => {
                getVotes();
            });
    });

    document.getElementById("upVote").addEventListener('click', function (e) {
        changeBtnColors(e.target, true);

        fetch(`/api/Votes/`,
            {
                method: "POST",
                headers: {
                    'Content-Type': 'application/json;charset=UTF-8',
                    'RequestVerificationToken': token
                },
                body: JSON.stringify({ ForUserUserName: `${username}`, VoteValue: true })
            })
            .then((response) => {
                return response.text();
            })
            .then((result) => {
                fetch(`/api/Votes/`)
                    .then((response) => {
                        getVotes();
                    })
            });
    });

    function changeBtnColors(target, voteType) {
        const upVote = document.getElementById("upVote");
        const downVote = document.getElementById("downVote");
        const upVoteBtnColor = "votes-positive-btn";
        const downVoteBtnClass = "votes-negative-btn";


        function clearBtnsColors() {
            upVote.className = upVote.className.replace(upVoteBtnColor, "");
            downVote.className = downVote.className.replace(downVoteBtnClass, "");
        }

        if (downVote.className.includes(downVoteBtnClass) || upVote.className.includes(upVoteBtnColor)) {
            let oneOftheBtnsPressed = false;

            if (!target.className.includes(upVoteBtnColor) && !target.className.includes(downVoteBtnClass)) {
                oneOftheBtnsPressed = true;
            }

            clearBtnsColors();

            if (oneOftheBtnsPressed) {
                target.className = `${target.className} ${voteType ? upVoteBtnColor : downVoteBtnClass}`;
            }
        }
        else {
            target.className = `${target.className} ${voteType ? upVoteBtnColor : downVoteBtnClass}`;
        }
    }
}