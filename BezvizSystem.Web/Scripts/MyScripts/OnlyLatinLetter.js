$('input.asci_only').keyup(limitInput);

function limitInput()
{
    this.value = this.value.replace(/[^a-zA-Z0-9 -]/ig, '');
}