﻿// Copyright © 2015 - Present RealDimensions Software, LLC
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// 
// You may obtain a copy of the License at
// 
// 	http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace chocolatey.package.verifier.infrastructure.app.tasks
{
    using System;
    using infrastructure.messaging;
    using infrastructure.tasks;
    using messaging;
    using services;

    public class CreateGistTask : ITask
    {
        private IDisposable _subscription;
        private readonly IGistService _gistService;

        public CreateGistTask(IGistService gistService)
        {
            _gistService = gistService;
        }

        public void initialize()
        {
            _subscription = EventManager.subscribe<PackageTestResultMessage>(create_gist, null, null);
            this.Log().Info(() => "{0} is now ready and waiting for PackageTestResultMessage".format_with(GetType().Name));
        }

        public void shutdown()
        {
            if (_subscription != null) _subscription.Dispose();
        }

        private async void create_gist(PackageTestResultMessage message)
        {
            this.Log().Info(
                () => "Creating gist for Package: {0} Version: {1}".format_with(message.PackageId, message.PackageVersion));

            var gistDescription = "Test results for {0} Version {1}".format_with(message.PackageId, message.PackageVersion);

            var createdGistUrl = await _gistService.CreateGist(gistDescription, true, message.Logs);

            EventManager.publish(new GistCreateMessage(createdGistUrl.ToString()));
        }
    }
}
