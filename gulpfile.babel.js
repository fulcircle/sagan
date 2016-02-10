'use strict';

import gulp from 'gulp'
import sourcemaps from 'gulp-sourcemaps'
import uglify from 'gulp-uglify'
import babel from 'gulp-babel'
import babelify from 'babelify'
import browserify from 'browserify'
import buffer from 'vinyl-buffer'
import source from 'vinyl-source-stream'
import browserSync from 'browser-sync'

const dirs = {
	src: 'src',
	dst: 'static'
};

const paths = {
    js: {
        src: `${dirs.src}/js`,
        dst: `${dirs.dst}/js`
    }
};

gulp.task('js', () => {
    let bundler = browserify({
        entries: paths.js.src + '/app.js',
        debug: true
    });

    bundler.transform(babelify.configure({
        ignore: /three\.min\.js/
    }));

    bundler.bundle()
        .on('error', function (err) { console.error(err); })
        .pipe(source('app.js'))
        .pipe(buffer())
        //.pipe(uglify()) // Use any gulp plugins you want now
        .pipe(sourcemaps.init({ loadMaps: true }))
        .pipe(sourcemaps.write('./', {
            sourceMappingURLPrefix: '/js'
        }))
        .pipe(gulp.dest(paths.js.dst));
});

gulp.task('watch', function() {
    let bs = browserSync.create();
    bs.init({
        server: {
            baseDir: dirs.dst
        }
    });

    gulp.watch(paths.js.src + '/**/*.js', ['js']);
    gulp.watch(dirs.dst + '/**/*').on('change', bs.reload);
});

