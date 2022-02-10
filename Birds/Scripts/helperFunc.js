function play(x) {
  //  x = x.replace(" ", "_");
    var audio = new Audio(x);
    audio.play();
}

function changeImg(imgId, imgUrl) {
    var Image_Id = document.getElementById(imgId);
    Image_Id.src = imgUrl;
}
function changeTextStyle(id, e) {
    if (e === "i") {
        document.getElementById(id).style.color = "orange";
        document.getElementById(id).style.font = "italic bold 20px arial,serif";
    } else {
        document.getElementById(id).style.color = "black";
        document.getElementById(id).style.font = "normal bold 20px arial,serif";
    }
}