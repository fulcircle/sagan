export function* randomNumber(min, max) {
    while (true) {
        yield (Math.random() * (max - min) + min);
    }
}

export function initArray(length) {
    var arr = new Array(length || 0),
        i = length;

    if (arguments.length > 1) {
        var args = Array.prototype.slice.call(arguments, 1);
        while(i--) arr[length-1 - i] = initArray.apply(this, args);
    }

    return arr;
}

