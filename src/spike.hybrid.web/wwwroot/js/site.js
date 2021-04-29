// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.
async function updateDbStatus(elementId) {
    document.getElementById(elementId).innerText = 'Checking database connection...';
    let response = await checkDb();    
    document.getElementById(elementId).innerText = response.message;
}
async function checkDb() {

    let response = await fetch('/api/health/db');

    if (response.ok == true) {
        let json = await response.json();
        return json;
    }
    return { success: false };
}

async function updateSmtpStatus(elementId) {
    document.getElementById(elementId).innerText = 'Checking smtp service...';
    let response = await checkSmtp();
    document.getElementById(elementId).innerText = response.message;
}
async function checkSmtp() {

    let response = await fetch('/api/health/smtp');

    if (response.ok == true) {
        let json = await response.json();
        return json;
    }
    return { success: false };
}