const bckCover = "https://birds.grynberg.dk/images/BackCover.png";
const empty = "https://birds.grynberg.dk/images/EmptySpace.jpg";
const nopSound = "https://birds.grynberg.dk/SoundTracks/songs/nope.mp3";
const baseSoundUrl = "https://birds.grynberg.dk/SoundTracks/";
const baseImageUrl = 'https://birds.grynberg.dk/images/';
const genius = baseImageUrl + 'albert.png';
const mp3SoundType = ".mp3";
let flippedItems = [];


function flipImage(id, urlImg, reader, cheatMode = "yes") {
    console.log('kuk');
    flippedItems = document.querySelectorAll("[flipped='y']");

    let choosenCords = flippedItems.length;
    let chM = (cheatMode == "yes");
    let audio = new Audio(reader);

    if (choosenCords > 1) { return; }
    let currentImg = document.getElementById(id);

    if (currentImg.getAttribute('state') == "clicked") {
        return;
    }

    if (choosenCords > 0) {
        flipedImg = document.getElementById(flippedItems[0].getAttribute('id'));
    }

    if (currentImg.getAttribute('src') == bckCover) {
        if ((choosenCords > 0) && (currentImg.getAttribute('lang') == flipedImg.getAttribute('lang')) && (chM)) {
            var alertLang = currentImg.getAttribute('lang');
            console.log(alertLang);
            if (alertLang == "en") {
                alert("Language can\'t be the same for both cards!.");
            }
            else if (alertLang == "dk") {
                alert("Sproget skal må ikke være det samme for begge kort!.");
            }
            return;
        }
        currentImg.setAttribute('src', urlImg);
        currentImg.setAttribute('flipped', "y");
        audio.play();
        currentImg.setAttribute('state', 'clicked')
        choosenCords++;
    }

    if (currentImg.getAttribute('src') == empty) {
        return;
    }

    if (choosenCords > 1) {
        setTimeout(function () { checkMatch(); }, 2000);
    }
}


function checkMatch() {
    flippedItems = document.querySelectorAll("[flipped='y']");
    let soundTrak = "";
    let trials = 0;
    let hits = 0;
    let trialsTmp = document.getElementById("trial");
    trials = parseInt(trialsTmp.getAttribute("data-const"));
    let hitTmp = document.getElementById("hit");
    hits = parseInt(hitTmp.getAttribute("data-const"));
    let gameover = false;
    let pNode = flippedItems[0].parentNode;

    if (flippedItems.length > 1) {
        if (flippedItems[0].getAttribute("latinName") == flippedItems[1].getAttribute("latinName")) {
            soundTrak = flippedItems[0].getAttribute("bird-voice");
            let audio = new Audio(soundTrak);
            let innerDiv1 = document.getElementById(flippedItems[0].parentNode.parentNode.getAttribute("id")).childNodes;
            let innerDiv2 = document.getElementById(flippedItems[1].parentNode.parentNode.getAttribute("id")).childNodes;

            audio.play();
            setTimeout(function () {
                // maybe I rewrite this into an explicit function one day.
                innerDiv1.forEach((itm) => {
                    if (itm.getAttribute("id") == "innerDiv") {
                        itm.textContent = "";
                    }
                });
                innerDiv2.forEach((itm) => {
                    if (itm.getAttribute("id") == "innerDiv") {
                        itm.textContent = "";
                    }
                });

                audio.pause(); audio.currentTime = 0;
                flippedItems[0].setAttribute('src', empty);
                flippedItems[1].setAttribute('src', empty);
                flippedItems[0].setAttribute('flipped', "n");
                flippedItems[1].setAttribute('flipped', "n");
                flippedItems[0].setAttribute('title', "The birds has flown.");
                flippedItems[1].setAttribute('title', "Fuglen er fløjet.");


                while (pNode.getAttribute("class") != "animPic") {
                    pNode = pNode.parentNode;
                }
                pNode.classList.remove('animPic');

                pNode = flippedItems[1].parentNode;
                while (pNode.getAttribute("class") != "animPic") {
                    pNode = pNode.parentNode;
                }
                pNode.classList.remove('animPic');

                hits++;
                hitTmp.setAttribute("data-const", hits.toString());
                hitTmp.innerHTML = hits.toString();
                gameover = gameOverCheck();
            }, 5000);
        }
        else {
            let audio = new Audio(nopSound);

            audio.play();
            setTimeout(function () {
                audio.pause(); audio.currentTime = 0;
                flippedItems[0].setAttribute('src', bckCover);
                flippedItems[1].setAttribute('src', bckCover);
                flippedItems[0].setAttribute('flipped', "n");
                flippedItems[1].setAttribute('flipped', "n")
                flippedItems[0].setAttribute('state', 'unclicked');
                flippedItems[1].setAttribute('state', 'unclicked');
            }, 4000);
        }
        trials++;
        trialsTmp.setAttribute("data-const", trials.toString());
        trialsTmp.innerHTML = trials.toString();
    }
}

function gameOverCheck() {
    let trickCount = document.querySelectorAll("[src='" + empty + "']").length;
    let imagesCount = document.getElementsByClassName('BirdImg').length;

    if (trickCount == imagesCount) {
        let tab = document.getElementById("gameBord");
        tab.innerHTML = "";
        let div = document.createElement('div');
        div.className = "Gameover";
        div.innerHTML = "The game is over";
        tab.appendChild(div);
        let res = document.createElement('div');
        res.className = "Result";
        res.classList.add("animPic");
        let albert = document.createElement('img');
        albert.setAttribute('src', genius);
        tab.appendChild(res);
        res.innerHTML = " but you are a genius!";
        res.appendChild(albert);
        return true;
    }
    return false;
}

function playSound(audioUrl, duration = 0) {
    let audio = new Audio(audioUrl)

    audio.play();
    if (duration > 0) {
        setTimeout(function () {
            audio.pause(); audio.currentTime = 0;
        }, duration)
    };
}

function playThis(obj) {
    let soundUrl = "";
    let altText = obj.alt;
    console.log(obj);
    console.log(altText);
    if ((altText == 'LevelDropDown') || (altText == 'LanguageDropDown')) {
        let el = document.getElementById(altText).value;
        soundUrl = baseSoundUrl + "en/" + "mega" + el + mp3SoundType;

    }
    else {
        let fileOrgin = obj.alt.split("_");
        soundUrl = baseSoundUrl + fileOrgin[1] + "/" + fileOrgin[0] + mp3SoundType;
    }
    console.log(soundUrl);
    playSound(soundUrl, 0);
}
