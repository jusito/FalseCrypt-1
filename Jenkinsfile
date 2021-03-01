pipeline {
    agent any

    stages {
        stage('GradleBuild') {
            steps {
                cd 'java_android'
                sh './gradlew clean build'
            }
        }
    }
}
