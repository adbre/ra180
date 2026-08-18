export default {
    test: {
        environment: 'node',
        globals: true,
        include: ['Html/spec/**/*Spec.js'],
        setupFiles: ['Html/spec/setup.js'],
    },
};
