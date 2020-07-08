function validateCardInput() {
    var cardName = document.getElementById("Name").value;
    var cardDate = document.getElementById("Date").value;
    var cardCVC = document.getElementById("CVC").value;
    const digits_only = string => [...string].every(c => '0123456789'.includes(c));
    if ((cardName.toString() == '') || (cardName.toString().length != 16) || (!digits_only(cardName.toString()))) {
        swal("Error", "Please enter a valid card number", "error")
        return false;
    }
    else {
        if ((cardDate.toString() == '') || (cardDate.toString().length != 5)) {
            swal("Error", "Please enter a valid expiration date", "error")
            return false;
        }
        else {
            if ((cardCVC.toString() == '') || (cardCVC.toString().length != 3) || (!digits_only(cardCVC.toString()))) {
                swal("Error", "Please enter a valid CVC", "error")
                return false;
            }
            else {
                return true;
            }
        }
    }
}