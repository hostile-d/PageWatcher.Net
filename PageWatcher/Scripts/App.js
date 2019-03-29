const dateElement = document.getElementById('date');
if (dateElement) {
    const timeStamp = dateElement.dataset.timestamp;
    const time = moment(timeStamp).local().fromNow();
    dateElement.innerText = 'Last time updated ' + (time || 'never');
}
